#include "scout.h"


scout::scout(string _name, ofVec2f start) : busTX(_name)
{ 
	pos = start;
	name = _name;
	changeState(0);
	fleeCounter = 0;
}

scout::~scout()
{
}

void scout::move(int dx, int dy)
{
	//Restrain scouts to the grid
	if (pos.x + dx > 15 ||
		pos.x + dx < 0 ||
		pos.y + dy > 15 ||
		pos.y + dy < 0)
	{
		cout << name << "Tried to move OOB." << endl;
	}
	else
	{
		pos = pos + ofVec2f(dx, dy);
	}
}

//Forces the scout to move in random directions
//if it is colliding with another scout
//until they are both free
void scout::collisionCheck()
{
	int repeats = 0;
	for (int i = 0; i < occupiedPositions.size(); i++)
	{
		if (occupiedPositions.at(i) == pos) repeats++;
	}

	if (repeats > 1)
	{
		int dx = 1;
		int dy = 1;
		if ((int)ofRandom(100) % 2) dx = -1;
		if ((int)ofRandom(100) % 2) dy = -1;
		move(dx, dy);
	}
}

void scout::reportPos()
{
	broadcast("moved " + to_string(pos.x) + " " + to_string(pos.y));
}

void scout::changeState(int newState)
{
	state = newState;

	if (state == 3) fleeCounter = 5;
	//Do more
}

void scout::handleState()
{
	if (state == 0)
	{
		//Hanging out...
	}
	else if (state == 1)
	{
		//Not implemented
	}
	else if (state == 2)
	{
		//seek behavior
		if (pos.distance(effect) < 4)
		{
			changeState(0);
		}
		else
		{
			int dx = 0;
			int dy = 0;

			if (pos.x > effect.x) dx = -1;
			else if (pos.x < effect.x) dx = 1;

			if (pos.y > effect.y) dy = -1;
			else if (pos.y < effect.y) dy = 1;

			move(dx, dy);
			if (pos.distance(effect) < 4)
			{
				changeState(0);
			}
		}
	}
	else if (state == 3)
	{
		//Flee behavior
		int dx = 0;
		int dy = 0;

		if (pos.x > effect.x) dx = 1;
		else if (pos.x < effect.x) dx = -1;

		if (pos.y > effect.y) dy = 1;
		else if (pos.y < effect.y) dy = -1;

		move(dx, dy);

		fleeCounter--;
		if (fleeCounter <= 0) changeState(0);
	}
}

void scout::forgetOccupied()
{
	occupiedPositions.clear();
}


void scout::draw()
{
	if (state == 0 || state == 1) ofSetColor(0, 0, 0);
	if (state == 2) ofSetColor(0, 0, 255);
	if (state == 3) ofSetColor(255, 0, 0);
	ofDrawCircle(125 + pos.x * 50, 125 + pos.y * 50, 12);
}


void scout::handleBusMessages()
{
	pair<string, string> messagep;
	messagep = busRX.getMessage();
	string message = messagep.first;
	string sender = messagep.second;

	while (message != "null")
	{
		//Check if the message matters to a scout
		stringstream reader(message);
		string parse;
		reader >> parse;

		if (sender.find("scout") != sender.npos)
		{
			//Do we recognize the message?

			if (parse == "moved") //Commit another scout's position to memory
			{
				reader >> parse;
				int x = stoi(parse);
				reader >> parse;
				int y = stoi(parse);

				occupiedPositions.push_back(ofVec2f(x, y));
			}
			else
			{
				//ignore
			}
		}
		else if (sender == "sound")
		{
			if (parse == "good" || parse == "bad")
			{
				if (parse == "good") changeState(2);
				if (parse == "bad") changeState(3);

				reader >> parse;
				int x = stoi(parse);
				reader >> parse;
				int y = stoi(parse);

				effect = ofVec2f(x, y);
			}
			else
			{
				//ignore
			}
		}
		else
		{
			cout << "Scout ignored databus message from " << sender << endl;
		}


		//Get next message to process...
		messagep = busRX.getMessage();
		message = messagep.first;
		sender = messagep.second;
	}
}

dataBusReceiver* scout::getBusReceiverPtr() { return (&busRX); }

void scout::broadcast(string message)
{
	busTX.send(message);
}

void scout::setBus(dataBus* bus)
{
	busTX.setCarrier(bus);
}


ofVec2f scout::getPos() { return pos; }