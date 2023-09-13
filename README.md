# GameFramework
An extensible framework for many different two-player board games implemented using C# on .Net 6.

## Description
A variety of design patterns were used to achieve extensibility and reusability of the framework.
Demonstration of the afformentioned properties was done by implementing the following games.

* SOS. Two players take turns to add either an S or an O (no requirement to use the same
letter each turn) on a board with at least 3x3 squares in size. If a player makes the sequence
SOS vertically, horizontally or diagonally they get a point and also take another turn. Once the
grid has been filled up, the winner is the player who made the most SOSs.
* Connect Four aka Four in a Row. Two players take turns dropping pieces on a 7x6 board. The
player forms an unbroken chain of four pieces horizontally, vertically, or diagonally, wins the
game.
