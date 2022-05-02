#include <ofMain.h>
#include "dataBusReceiver.h"
#include "dataBusSender.h"
#include <sstream>

#pragma once
class scout
{
private:
	string name;
	ofVec2f pos;
	ofVec2f effect;
	vector<ofVec2f> occupiedPositions;
	int state;
	// 0 = idle
	// 1 = wait (NOT IMPLEMENTED)
	// 2 = move towards location
	// 3 = flee from location
	dataBusReceiver busRX;
	dataBusSender busTX;

	int fleeCounter;

public:
	scout(string _name, ofVec2f start);
	~scout();

	void move(int dx, int dy);
	void collisionCheck(); //Vibe check
	void reportPos();

	void changeState(int newState);
	void handleState();
	void forgetOccupied();

	void draw();

	void handleBusMessages();
	dataBusReceiver* getBusReceiverPtr();
	void broadcast(string message);
	void setBus(dataBus* bus);

	ofVec2f getPos();
};

