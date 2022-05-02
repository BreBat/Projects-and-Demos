#include "ofApp.h"

//--------------------------------------------------------------
void ofApp::setup(){
	ofSetBackgroundColor(180, 180, 180);

	testDataBusStuff();
	cout << endl << endl << endl;

	scouts.push_back(scout("scout1", ofVec2f(2, 4)));
	scouts.push_back(scout("scout2", ofVec2f(10, 14)));
	scouts.push_back(scout("scout3", ofVec2f(6, 6)));

	for (int i = 0; i < scouts.size(); i++)
	{
		events.subscribe(scouts.at(i).getBusReceiverPtr());
		scouts.at(i).setBus(&events);
		scouts.at(i).reportPos();
	}

	ofSeedRandom(time(0));

}

//--------------------------------------------------------------
void ofApp::update(){

	if (ofGetElapsedTimef() - lastUpdate > 0.5f)
	{
		cout << endl << endl;
		//Sounds trigger
		for (int i = 0; i < sounds.size(); i++)
		{
			sounds.at(i).decay(); //New sounds trigger a bus event, old sounds fade off screen
		}

		//Scouts check data bus for new sounds, set state accordingly
		for (int i = 0; i < scouts.size(); i++)
		{
			scouts.at(i).handleBusMessages(); //Listen for sounds
		}

		//Scouts move according to state 
		for (int i = 0; i < scouts.size(); i++)
		{
			scouts.at(i).handleState(); //Movement
			scouts.at(i).forgetOccupied(); //Forget where friends were
			scouts.at(i).reportPos(); //Broadcast new positions
		}

		//Scouts deal with collision
		for (int i = 0; i < scouts.size(); i++)
		{
			scouts.at(i).handleBusMessages(); //Record new positions
			scouts.at(i).collisionCheck(); //Stop scouts from overlapping
		}

		//Scouts share new positions
		for (int i = 0; i < scouts.size(); i++)
		{
			scouts.at(i).forgetOccupied(); //End of round position sharing
			scouts.at(i).reportPos(); //Broadcast new positions
		}
		

		lastUpdate = ofGetElapsedTimef();
	}
}

//--------------------------------------------------------------
void ofApp::draw(){

	for (int i = 0; i <= 16; i++)
	{
		ofSetColor(0, 0, 0);
		ofSetLineWidth(3.5);
		ofDrawLine(100, 100 + (i * 50), 900, 100 + (i * 50));
		ofDrawLine(100 + (i * 50), 100 , 100 + (i*50), 900);
	}

	for (int i = 0; i < scouts.size(); i++)
	{
		scouts.at(i).draw();
	}
	for (int i = 0; i < sounds.size(); i++)
	{
		sounds.at(i).draw();
	}
}

//--------------------------------------------------------------
void ofApp::keyPressed(int key){

}

//--------------------------------------------------------------
void ofApp::keyReleased(int key){

}

//--------------------------------------------------------------
void ofApp::mouseMoved(int x, int y ){

}

//--------------------------------------------------------------
void ofApp::mouseDragged(int x, int y, int button){
	//cout << mouseX << ", " << mouseY << endl;
}

//--------------------------------------------------------------
void ofApp::mousePressed(int x, int y, int button){

	if (x < 100 || x > 900 || y < 100 || y > 900)
	{
		//ignore
	}
	else
	{
		int clickX = (x - 100) / 50;
		int clickY = (y - 100) / 50;

		if (button == 0)
		{
			//good sound
			sounds.push_back(sound("good", ofVec2f(clickX, clickY)));
			sounds.back().setBus(&events);
		}
		else if (button == 2)
		{
			//bad sound
			sounds.push_back(sound("bad", ofVec2f(clickX, clickY)));
			sounds.back().setBus(&events);
		}

	}
	
}

//--------------------------------------------------------------
void ofApp::mouseReleased(int x, int y, int button){

}

//--------------------------------------------------------------
void ofApp::mouseEntered(int x, int y){

}

//--------------------------------------------------------------
void ofApp::mouseExited(int x, int y){

}

//--------------------------------------------------------------
void ofApp::windowResized(int w, int h){

}

//--------------------------------------------------------------
void ofApp::gotMessage(ofMessage msg){

}

//--------------------------------------------------------------
void ofApp::dragEvent(ofDragInfo dragInfo){ 

}
