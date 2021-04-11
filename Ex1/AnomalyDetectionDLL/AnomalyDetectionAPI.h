#pragma once
#include <vector>
#include <string>
#include "timeseries.h"
#include "SimpleAnomalyDetector.h"

class stringWrapper {
	std::string str;
public:
	stringWrapper() { }
	void setString(std::string str) {
		this->str = str;
	}
	int len() {
		return str.size();
	}
	char getCharByIndex(int x) {
		return str[x];
	}
};

extern "C" __declspec(dllexport) void* CreatestringWrapper() {
	return (void*) new stringWrapper;
}

extern "C" __declspec(dllexport) int len(stringWrapper * s) {
	return s->len();
}

extern "C" __declspec(dllexport) char getCharByIndex(stringWrapper * s, int x) {
	return s->getCharByIndex(x);
}

class VectorWrapper {
	std::vector<stringWrapper*> vec;
public:
	VectorWrapper() {}
	void addString(std::string str) {
		stringWrapper* strWrap = new stringWrapper();
		strWrap->setString(str);
		vec.push_back(strWrap);
	}
	int VecSize() {
		return vec.size();
	}

	stringWrapper* getByIndex(int x) {
		return vec[x];
	}
	~VectorWrapper() {
		for (stringWrapper* s : vec)
			delete s;
	}
};
extern "C" __declspec(dllexport) void* CreateVectorWrapper() {
	return (void*) new VectorWrapper;
}

extern "C" __declspec(dllexport) int vectorSize(VectorWrapper * v) {
	return v->VecSize();
}

extern "C" __declspec(dllexport) void* getByIndex(VectorWrapper * v, int index) {
	return (void*)v->getByIndex(index);
}

class API {
	VectorWrapper correlationNames;
	VectorWrapper regLines;
public:
	API(const char* fileName) {
		TimeSeries ts(fileName);
		SimpleAnomalyDetector ad;
		ad.learnNormal(ts);
		std::vector<correlatedFeatures> cf = ad.getNormalModel();
		for (correlatedFeatures c : cf)
		{
			correlationNames.addString(c.feature1 + "," + c.feature2);
			regLines.addString(std::to_string(c.lin_reg.a) + "," + std::to_string(c.lin_reg.b));
		}
	}
	VectorWrapper* getCorrelationNamesVector() {
		return &correlationNames;
	}
	VectorWrapper* getRegLinesVector() {
		return &regLines;
	}
};

extern "C" __declspec(dllexport) void* CreateAPI(const char* fileName) {
	cout << "API" << endl;
	API* api = new API(fileName);
	return (void*)api;
}

extern "C" __declspec(dllexport) void* getCorrelationNamesVector(API * api) {
	return (void*)api->getCorrelationNamesVector();
}

extern "C" __declspec(dllexport) void* getRegLinesVector(API * api) {
	return (void*)api->getRegLinesVector();
}