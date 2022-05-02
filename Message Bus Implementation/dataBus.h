#include <ofMain.h>
#include "dataBusMessage.h"
#include "dataBusReceiver.h"

#pragma once
class dataBus
{
private:
	vector<dataBusReceiver*> subscribers;
	int findSubscriber(dataBusReceiver* sub);

public:
	dataBus();
	~dataBus();

	void publish(string msg, string aut);
	void subscribe(dataBusReceiver* sub);
	void unsubscribe(dataBusReceiver* sub);
};

