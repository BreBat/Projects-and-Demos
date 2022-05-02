#include "sound.h"



sound::sound(string _type, ofVec2f start) : emitter("sound")
{
	type = _type;
	pos = start;
	played = false;
	active = true;

	if (type == "bad") sprite.load("sprites/bad.png");
	else sprite.load("sprites/good.png");

	if (sprite.isAllocated() == false) throw;

	lifespan = 4;
}


sound::~sound()
{
}

void sound::setBus(dataBus* bus)
{
	emitter.setCarrier(bus);
}


void sound::decay()
{
	if (!played)
	{
		emitter.send(type + " " + to_string(pos.x) + " " + to_string(pos.y));
		played = true;
		return;
	}
	lifespan--;
	if (lifespan == 0) active = false;
}

void sound::draw()
{
	if (active)
	{
		ofSetColor(255, 255, 255);
		sprite.draw(100 + pos.x * 50, 100 + pos.y * 50, 49, 49);
	}
}