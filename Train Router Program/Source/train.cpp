#include "train.h"
#include <math.h>
#include <time.h>


train::train(string _name, int ID, loadType _type, int speed, node* homeHub, int cap, float fuelUse, float price)
{
	trainStats first;
	stats.push_back(first);

	name = _name;
	uniqueID = ID;
	Ltype = _type;
	travelSpeed = speed;
	travelSpeedPerMinute = (float)speed / 60;
	home = homeHub;
	capacity = cap;
	fuelPerWeight = fuelUse;

	state = IDLE;
	exitState = IDLE;
	location = home;
	home->arrival(this);
	direction = NULL;
	progress = 0.0;
	fuel = 100.0;
	crewtime = 0;
	waitTime = 0;
	waitProgress = 0;
	loadCarried = NULL;
	loadSought = NULL;
	cargoPrice = price;
}

train::~train()
{
}

//TESTING UTILITY DO NOT USE
void train::teleport(node* dest)
{
	if (dest != NULL) 
	{
		location->departure(this);
		location = dest;
		location->arrival(this);
		cout << "DEBUG: " << name << " teleported to " << location->getName() << endl;
	}
}

void train::teleport(node* dest, node* heading)
{
	if (dest != NULL)
	{
		location->departure(this);
		location = dest;
		location->arrival(this);
		direction = heading;
		cout << "DEBUG: " << name << " teleported to " << location->getName() << " facing toward " << direction->getName() << endl;
	}
}


void train::changeLocation()
{
	if (direction == NULL)
	{
		cout << name << " attempted to change to a NULL direction" << endl;
	}
	else
	{
		//cout << "DEBUG: Old loc: " << location->getName() << "  Old dir: " << direction->getName() << endl;

		if (location->getNodeType() == HUB || location->getNodeType() == STATION)
		{
			location->departure(this);	//Exit station
			location = direction;		//enter track
			location->arrival(this);	//inform track
			direction = currentPath[currentPath.size() - 1].thisNode; //Get new direction from path
			//cout << "DEBUG: New loc: " << location->getName() << "  new dir: " << direction->getName() << endl;
		}
		else
		{
			location->departure(this);	//Exit track
			location = direction;		//enter station
			location->arrival(this);	//Inform station
			if (currentPath.size() > 1) //If we aren't at the end of the line
			{
				node* nextDirection = getNextTrackHeading(); //Find next direction
				direction = nextDirection;
				//cout << "DEBUG: New loc: " << location->getName() << "  new dir: " << direction->getName() << endl;
			}
			else //If we are done with the route
			{
				direction = NULL;
			}
			currentPath.pop_back();
		}
	}
}


loadType train::getType() { return Ltype; }

trainState train::getExitState() { return exitState; }

trainState train::getState() { return state; }

string train::getName() { return name; }

string train::getStateName()
{
	switch (state)
	{
	case IDLE:
		return "Idle";

	case SEEK:
		return "Seeking load";

	case HAUL:
		return "Hauling load";

	case WAIT:
		return "Waiting (" + to_string(waitTime - waitProgress) +")";

	case FUEL:
		return "Refueling";

	case HOME:
		return "Returning to home hub";

	case MAIN:
		return "Down for maintenance";

	default:
		cout << "Train::getStateName defaulted. This should never happen" << endl;
		return "HAS AN ERROR";
	}
}

node* train::getHeading()
{
	if (direction != NULL)
	{
		return direction;
	}
	else
	{
		cout << "train::getHeading called when train had no destination" << endl;
		return NULL;
	}
}

node* train::getLocation()
{
	if (location != NULL)
	{
		return location;
	}
	else
	{
		cout << "train::getLocation called, train has null location." << endl;
		return NULL;
	}
}

node* train::getHome()
{
	if (home == NULL)
	{
		cout << name << "'s home was NULL, somehow." << endl;
		return NULL;
	}
	else
	{
		return home;

	}
}

node * train::getDestination()
{
	if (destination == NULL)
	{
		cout << "train::getDestination called when destination was NULL" << endl;
		return NULL;
	}
	else
	{
		return destination;
	}
}

bool train::hasLoad() { return (loadCarried != NULL); }

bool train::hasPath() { return (currentPath.empty() == false); }

float train::getFuel() { return fuel; }

load* train::getLoadCarried() { return loadCarried; }

load* train::getLoadSought() { return loadSought; }


void train::printInfo()
{
	cout << PIPE_VERT;
	cout << setfill(' ') << left;

	string loadName = "None";
	if (loadCarried != NULL)
	{
		loadName = loadCarried->getName() + " (" + to_string(loadCarried->getAmount()) + ")";
		if (loadCarried->getAmount() == capacity) loadName = loadName + " (MAX)";
	}



	string typeString = "(Error)";
	if (Ltype == PASSENGER) typeString = "(Passenger)";
	if (Ltype == FREIGHT) typeString =   "  (Freight)";

	cout << name << ", ID: " << uniqueID << " " << typeString << " Home: " << home->getName() << "  Crew time: " << crewtime << "min " << " Fuel: " << setprecision(3) << fuel << "%  State: " << getStateName();

	cout << endl << PIPE_TR << PIPE_HORZ << PIPE_HORZ << PIPE_DLR;
	cout << "Location: " << location->getName();

	cout << endl << "   " << PIPE_UDR << PIPE_HORZ << PIPE_HORZ << PIPE_HORZ << PIPE_HORZ;
	cout << "Load: " << loadName;

	if (direction != NULL)
	{
		int bars = (int)progress;
		float fraction = (float)progress - bars;
		bars = bars / 10;

		char shade = 'x';
		if (fraction == 0.00f) shade = '.';
		if (fraction > 0.00f) shade = SHADE1;
		if (fraction > 0.33f) shade = SHADE2;
		if (fraction > 0.66f) shade = SHADE3;

		cout << endl << "   " << PIPE_UDR << PIPE_HORZ;
		cout << "Heading: " << setw(10) << direction->getName() << " ";

		for (int i = 0; i < bars; i++) cout << SHADE4;
		cout << shade;
		for (int i = 9 - bars; i > 0; i--) cout << ".";
		cout << endl << endl;

	}
}

int train::getCrewTime()
{
	return crewtime;
}

void train::printRoute()
{
	if (currentPath.empty()) cout << "train::printRoute called on train " << name << " which has no route" << endl;
	else
	{
		for (int i = 0; i < currentPath.size(); i++)
		{
			cout << currentPath[i].thisNode->getName() << "(W" << currentPath[i].weightBefore << "  TD" << currentPath[i].distance << ")";

			if (i + 1 < currentPath.size()) cout << ", ";
		}
		cout << endl;
	}
}


void train::deleteRoute()
{
	currentPath.clear();
}

void train::setState(trainState newState)
{
	if (newState < 0 || newState > 6) 
		cout << "Tried to assign invalid state to train " << name << endl;

	else
	{
		//cout << endl << "STATE CHANGE FOR " << name << ": FROM " << getStateName() << " TO ";
		state = newState;
		//cout << getStateName() << endl;
	}
}

void train::setExitState(trainState newExitState)
{
	exitState = newExitState;
}

void train::setPath(vector<route::record> newRoute) 
{ 
	if (newRoute.size() <= 1)
	{
		cout << "train::setPath encountered a route with one or less entries given to " << name << " at " << location->getName() << endl;
	}
	else if (location != newRoute[newRoute.size() - 1].thisNode)
	{
		cout << "train::setPath passed a route to " << name << " that does not begin at its location " << location->getName() << endl;
	}
	else
	{
		currentPath = newRoute;
		currentPath.pop_back();
		direction = getNextTrackHeading();
		destination = currentPath[0].thisNode; //Set the destination
	}

}

void train::seekLoad(load* newLoad)
{
	if (loadCarried != NULL)
	{
		if (loadCarried == newLoad)
		{
			cout << name << " was assigned the load that it already has" << endl;
		}
		else cout << name << " was assigned a new load despite already carrying one" << endl;
	}
	else
	{
		if (newLoad->getType() != Ltype)
		{
			cout << name << " was assigned a load of incorrect type." << endl;
		}
		else
		{
			loadSought = newLoad;
			destination = loadSought->getSpawn();
		}
	}
}

void train::pickupLoad(milTime when)
{
	if (loadSought == NULL)
	{
		cout << name << " attempted to pick up a load despite not having a sought load" << endl;
	}
	else if (loadCarried != NULL)
	{
		cout << name << " attempted to pick up a load despite already carrying one" << endl;
	}
	else if (Ltype == loadSought->getType())
	{
		loadCarried = loadSought;
		loadCarried->setPickupTime(when);
		loadSought = NULL;

		if (Ltype == FREIGHT)
		{
			int size = loadCarried->getAmount();
			stats[stats.size() - 1].totalCarried += size;
			if (size > stats[stats.size() - 1].maxCarried) stats[stats.size() - 1].maxCarried = size;

			stats[stats.size() - 1].loadRevenue += size * cargoPrice;
		}
	}
	else
	{
		cout << name << " attempted to pick up a load of incorrect load type" << endl;
	}
}

void train::dropoffLoad(milTime when)
{
	if (loadCarried == NULL)
	{
		cout << name << " attempted to drop off a load despite not having one" << endl;
	}
	else if (Ltype == loadCarried->getType())
	{
		loadCarried->setDropoffTime(when);
		if (Ltype == PASSENGER)
		{
			int amt = loadCarried->getAmount();
			loadCarried->subPassengers(amt);
		}
		loadCarried = NULL;
	}
	else
	{
		cout << name << " attempted to drop off a load of incorrect load type" << endl;
	}
}

void train::transferLoad(milTime when, int minOn, int maxOn, int minOff, int maxOff, float price)
{
	if (loadCarried == NULL)
	{
		cout << name << " attempted to transfer passengers despite not having a load." << endl;
	}
	else if (Ltype == loadCarried->getType())
	{
		srand(time(NULL));
		int amountOn = rand() % (maxOn - minOn + 1) + minOn;
		int amountOff = rand() % (maxOff - minOff + 1) + minOff;

		stats[stats.size() - 1].totalPassengers += amountOn;

		loadCarried->subPassengers(amountOff);
		if (amountOn + loadCarried->getAmount() > capacity) amountOn = capacity - loadCarried->getAmount();
		loadCarried->addPassengers(amountOn);

		stats[stats.size() - 1].loadRevenue += (amountOn * price);
		stats[stats.size() - 1].totalCarried += amountOn;
	}
	else
	{
		cout << name << " attempted to transfer passengers but has a load type mismatch with " << loadCarried->getName() << endl;
	}
}


void train::move(int* minutes)
{
	if (hasPath() == false)
	{
		cout << name << " was called to move but does not have a path right now." << endl;
		return;
	}
	if ((fuel > 0.0) && (crewtime <= 600))
	{
		//If we're on a hub, depart when safe
		if (location->getNodeType() == HUB || location->getNodeType() == STATION)
		{
			//Check for potential collisions, then embark if safe
			//Note: direction is already defined here
			bool safe = true;

			if (direction->getNodeType() == HUBTRACK);	//We're gauranteed to be safe if we are entering a hub con
			else										//Otherwise check for track activity
			{
				for (int i = 0; i < direction->getNumTrainsHere(); i++)
				{
					train* inspecting = direction->getTrain(i);
					if (inspecting->getHeading() == location) //If any train on the next track is coming towards us
					{
						safe = false;
						stats[stats.size() - 1].collisionsAvoided++;
					}
				}
			}
			if (safe)
			{
				//disembark location to direction (track). set new direction (node)
				changeLocation();
				progress = 0.00;
			}
			else
			{
				return; //keep waiting
			}

		}

		int weightTravelled = 0; //Used for stats

		//If we're on a track, keep moving
		if (location->getNodeType() == TRACK || location->getNodeType() == HUBTRACK)
		{
			int currentTrackWeight = currentPath[currentPath.size() - 1].weightBefore;

			weightTravelled = currentTrackWeight; //Used later, not here

			float progressPerMinute = (float)travelSpeed / currentTrackWeight;
			float distance = (float)travelSpeed / (60 * *minutes);
			//cout << "DEBUG: CTW: " << currentTrackWeight << "   PPM: " << progressPerMinute << endl;

			while (*minutes > 0 && progress < 99.99)
			{
				progress = progress + progressPerMinute;
				*minutes--;
				fuel = fuel - (distance * fuelPerWeight);
				if (fuel < 0.00) fuel = 0.00;

				stats[stats.size() - 1].fuelUsed += (distance * fuelPerWeight);
				stats[stats.size() - 1].upTime++;
			}
		}

		if (progress >= 99.99)	//If we have arrived at the next station/hub
								//Due to the nature of the progress meter, we can only enter this branch after leaving a track
		{
			stats[stats.size() - 1].distance += weightTravelled; //Update distance stat
			changeLocation();
			progress = 0.00;
			//currentPath.pop_back();

			//cout << "DEBUG: " << location->getName() << " vs " << destination->getName() << endl;
			if (location == destination) //end of line processing
			{
				if (state == SEEK || state == HAUL) state = WAIT;
				else if (state == FUEL); //Do nothing
				else if (state == HOME) state = IDLE;

				deleteRoute();
			}
			else //Prepare for next movement
			{
				node* target = currentPath[currentPath.size() - 1].thisNode;

				node* nextDir = getNextTrackHeading();


				if (nextDir == NULL)
				{
					cout << "train::Move could not find next track to assign to " << name << ". Infodump: " << endl;
					printInfo();
					cout << endl;
				}
				else
				{
					direction = nextDir;
				}
			}
		}
	}
	else
	{
		if (fuel <= 0)	cout << name << " tried to move but has been stranded without fuel." << endl;
		if (crewtime > 600)	cout << name << " tried to move but has exceeded legal crew uptime and has been stopped." << endl;
	}
}

node* train::getNextTrackHeading()
{
	node* result = NULL;

	if (location->getNodeType() == TRACK || location->getNodeType() == HUBTRACK)
	{
		cout << "train::getNextTrackHeading cannot get a new track heading if train is not at a node." << endl;
	}
	else
	{
		for (int i = 0; i < location->getNumConnections(); i++) //For all of our new location's tracks
		{
			node* inspectTrack = location->getConnection(i);
			for (int j = 0; j < 2; j++) //Check all of those tracks' connections
			{
				node* inspectNode = inspectTrack->getConnection(j);
				if (inspectNode == currentPath[currentPath.size() - 1].thisNode) //When we find the one that connects to our next path entry
				{
					result = inspectTrack; //Set is as our next destination;
				}
			}
		}
	}

	return result;
}


void train::setWait(int minutes, int weather)
{
	waitTime = minutes + weather;
	stats[stats.size() - 1].waitTimeWeather += weather;
}

void train::wait(int* minutes)
{
	if (state == WAIT)
	{
		while ((*minutes > 0) && (waitProgress < waitTime))
		{
			waitProgress++;
			*minutes = *minutes - 1;
		}

		if (waitProgress >= waitTime)
		{
			swapping = false; //If we were swapping crews, we aren't anymore
			state = exitState;
			waitTime = 0;
			waitProgress = 0;
		}
	}
	else //Auxiliary waits that aren't actually in the wait state
	{
		//Do nothing
	}
}

void train::refuel(float amount)
{
	fuel = fuel + amount;
	stats[stats.size() - 1].timesFuelled++;
	if (fuel > 100.0) fuel = 100.0;
}

void train::newCrew()
{
	crewtime = 0;
}

void train::addCrewTime(int minutes)
{
	crewtime += minutes;
}

void train::lockCrewTime()
{
	//Holds crewtime at 0 while swapping
	if (swapping) crewtime = 0;
}

void train::setSwapFlag() { swapping = true; }

void train::addDowntime(int amount) { stats[stats.size() - 1].downTime += amount; }

train::trainStats train::getTrainStats()
{
	trainStats result = stats[stats.size() - 1];
	stats.pop_back();
	return result;
}

void train::newDayTrain()
{
	trainStats newstat;
	stats.push_back(newstat);
}