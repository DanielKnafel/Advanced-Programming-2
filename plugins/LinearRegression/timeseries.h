/*
 * timeseries.h
 *
 *  Created on: 26 ����� 2020
 *      Author: Eli
 */

#ifndef TIMESERIES_H_
#define TIMESERIES_H_
#include <iostream>
#include <string>
#include <fstream>
#include <sstream>
#include<map>
#include <vector>
#include <string.h>
#include <algorithm>

using namespace std;

class TimeSeries{


	map<string,vector<float>> ts;
	vector<string> atts;
	size_t dataRowSize;
public:


	TimeSeries(const char* CSVfileName){
		ifstream in(CSVfileName);
		string head;
		in>>head;
		string att;
		stringstream hss(head);
		while(getline(hss,att,',')){
			vector<float> v;
			// element not found in map
			if (ts.find(att) != ts.end()) {
				att.append("[1]");
			}
			ts[att] = v;
			atts.push_back(att);
		}

		while(!in.eof()){
			string line;
			in>>line;
			string val;
			stringstream lss(line);
			int i=0;
			while(getline(lss,val,',')){
				ts[atts[i]].push_back(stof(val));
			     i++;
			}
		}
		in.close();

		dataRowSize = ts[atts[0]].size();

	}

	const vector<float>& getAttributeData(string name)const{
		return ts.at(name);
	}

	const vector<string>& gettAttributes()const{
		return atts;
	}

	size_t getRowSize()const{
		return dataRowSize;
	}

	~TimeSeries(){

	}
};



#endif /* TIMESERIES_H_ */
