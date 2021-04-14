#pragma once
#include "timeseries.h"
#include "SimpleAnomalyDetector.h"

using namespace System;

namespace AnomalyDetect {
	public ref class AnomalyDetector
	{
		SimpleAnomalyDetector* ad;
	public:
		AnomalyDetector();
		void learn(System::String^ learnFileName);
		cli::array<Tuple<System::String^, int>^>^ detect(System::String^ detectFileName);
		~AnomalyDetector();
	};
}
