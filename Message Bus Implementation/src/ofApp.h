#pragma once

#include "ofMain.h"
#include "../dataBus.h"
#include "../dataBusReceiver.h"
#include "../dataBusMessage.h"
#include "../scout.h"
#include "../sound.h"

class ofApp : public ofBaseApp{

	public:
		void setup();
		void update();
		void draw();

		void keyPressed(int key);
		void keyReleased(int key);
		void mouseMoved(int x, int y );
		void mouseDragged(int x, int y, int button);
		void mousePressed(int x, int y, int button);
		void mouseReleased(int x, int y, int button);
		void mouseEntered(int x, int y);
		void mouseExited(int x, int y);
		void windowResized(int w, int h);
		void dragEvent(ofDragInfo dragInfo);
		void gotMessage(ofMessage msg);
		
		float lastUpdate = 0;

		vector<scout> scouts;
		vector<sound> sounds;
		dataBus events;

		void testDataBusStuff()
		{
			dataBus testbus;
			dataBusReceiver one;
			dataBusReceiver two;
			dataBusReceiver three;

			testbus.subscribe(&one);
			testbus.subscribe(&two);
			testbus.subscribe(&three);

			testbus.publish("First Message", "Alpha");

			testbus.unsubscribe(&two);

			testbus.publish("Second Message", "Delta");
			testbus.unsubscribe(&two);
		}



};
