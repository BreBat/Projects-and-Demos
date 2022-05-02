#include "dataBus.h"

#pragma once
class dataBusSender
{
private:
	dataBus* carrier;
	string authorName;

public:
	dataBusSender(string name);
	~dataBusSender();

	void setCarrier(dataBus* bus);
	void send(string message);
};

