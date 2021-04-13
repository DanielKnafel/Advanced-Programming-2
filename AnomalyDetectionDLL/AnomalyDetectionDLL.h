#pragma once
#include "timeseries.h"
#include "SimpleAnomalyDetector.h"
using namespace System;

namespace AnomalyDetectionDLL {
	public ref class API
	{
		TimeSeries *ts;
		SimpleAnomalyDetector *ad;
		cli::array<System::Tuple<System::String^, System::String^>^> ^correlationNames;
		cli::array<System::Tuple<float, float>^> ^regLines;
	public:
		// TODO: Add your methods for this class here.
		API(System::String^);
		~API();
		cli::array<System::Tuple<System::String^, System::String^>^> ^ getCorrelationNamesVector();
		cli::array<System::Tuple<float, float>^> ^ getRegLinesVector();
		//getRegLinesVector
	};
}
