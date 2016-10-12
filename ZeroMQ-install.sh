#!/bin/bash

##############################################
#from http://zeromq.org/intro:get-the-software
##############################################

#get zeromq
wget https://github.com/zeromq/zeromq4-1/releases/download/v4.1.5/zeromq-4.1.5.tar.gz

#unpack tarball package
tar xvzf zeromq-4.1.5.tar.gz

#install dependency
sudo apt-get update && \
sudo apt-get install -y libtool pkg-config build-essential autoconf automake uuid-dev

#in zeromq dir
cd zeromq-4.1.5

#create make file
./configure

#build and install(root permission only)
sudo make install

#install zeromq driver on linux
sudo ldconfig
