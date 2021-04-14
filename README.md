# Advanced-Programming-2 - Flight Inspection App

## Introduction

In our project we connect to flight gear simulator.
We simulate a flight using a giving flight's csv files with necessary details regarding airplane's state
at any given second from the beginning - of airplane's taking off, till the end - of its landing.
Csv files include details the pilot or the autopilot are using such as:
Airplane's altitude, airspeed, pitch-roll-yaw degrees, joystick's details like aileron, elevator and rudder and so on.

We created fligth's controls pack which includes: 
1. Joystick that uses the ailerons, elevator and rudder to control the flight.
2. Dashboard to display the numerical data sent from the airplane.
3. Graph Control to display flight's correlated features and the detected anomalies.
4. Graph Regression displaying all points in comparsion with a pre-learned line regression. 
5. Video Control to play the recorded flight making the simulator come to live.
We use our project from the previous semester written in C++ in order to load it to this project so that we'll use
all the functionality for learning the anomalies at any desired flight. We are wrapped this C++ code with a dll (Dynamic Link Library) 
and loaded it to our current project.

The files we load to simulator are:

1. One file is a normal and regular flight containing all necessary flight's details using this file and our dll we learn how the correct flight supposed to be.

2. Second file is an abnormal flight which means a flight with anomaly, flawed flight. This time we supposed to detect the anomalies.

## Explanation of folders and main files structure
In the master we have the following files:

![folders](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/folders.png)

### Explanation of each folder
#### AnomalyDetectionDLL

![AnomalyDetectionDLL](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/anommalyDetectionDLL.png)

Contains the cpp files from the first semester for learning and detecting annomalies.
#### Ex1

![Ex1](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/Ex1.png)

Contains folders that will be explained after the explnation of the following files:

`CorrelatedFeaturesCalc.cs` - by given a file name, finds all correlated features. returns correlated feature for a given feature and also returns linear regression line
. 

`DataFileReader.cs` - by given a file name, reader the file and connect to the client. the client and the MainViewModel listens to him by getting events. 
it is the main part of the project that manages almost everthing(model).

`Line.cs` - represents a line by two points.

`MainViewModel.cs` - all the ViewModels, GraphControl, MainWindow listens to his according to the architecture of MVVM. listens to the DataFileReader. 

`MainWindow.xaml.cs` - The main view that contains all the controls of the project and displays them.

`playback_small.xml` - xml for the flight simulator.

##### client

![client](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/client.png)

The client that connects to the flight simulator server and listens to the DataFileReader.

##### images

![images](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/images.png)

Images for the controls.

##### controls

![controls](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/controls.png)

Contains all the Controls of the project.

Explanation of the controls:

`DashBoard`

![DashBoard](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/controls/dashboard.png)

Dashboard to display the numerical data sent from the airplane. Contains view and viewModel. the viewModel listens to MainViewModel. implents ViewModel abstract class.

Also has a class `KnotsToAnglesConverter` that converts from knots to angles. 

`GraphControl`

![GraphControl](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/controls/graphcontrol.png)

Graph Control to display flight's correlated features and the detected anomalies. Contains view that listens to MainViewModel.

`GraphReg`

![GraphReg](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/controls/graphreg.png)

Graph Regression displaying all points in comparsion with a pre-learned line regression. Contains view and viewModel. the viewModel listens to MainViewModel. implents ViewModel abstract class.


`Joystick`

![Joystick](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/controls/joystik.png)

Joystick that uses the ailerons, elevator and rudder to control the flight. Contains view, viewModel and model. the viewModel listens to MainViewModel. implents ViewModel abstract class.


`VideoControl`

![VideoControl](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/controls/videocontrol.png)

Video Control to play the recorded flight making the simulator come to live. Contains view, viewModel and model. the viewModel listens to MainViewModel. implents ViewModel abstract class.

`ViewModel.cs` 

abstract class for the ViewModels. containts the DataFileReader and implents INotifyPropertyChanged.
#### UML

Contains images of UMLs.

#### images
Contains images to display in README.md file.

#### plugins

![plugins](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/plugins.png)

Plugins to load into the project as dll. two plugins:

1. One file is a normal and regular flight containing all necessary flight's details using this file and our dll we learn how the correct flight supposed to be.

2. Second file is an abnormal flight which means a flight with anomaly, flawed flight. This time we supposed to detect the anomalies.

## preinstall for developers

## Installation and first run

## Links

## Video link

![Models](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/UML/Models.png)
