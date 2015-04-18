#!/bin/perl

use strict;
use warnings;

use Data::Dumper;

# Parses an .ini file into a hash with named sections
sub parseConfig {
    my ($filename) = @_;

    # Open the file if it exists
    if(! -e $filename) {
	print "'$filename' does not exist";
	die;
    }
    open(CONFIG, "<", $filename);

    my %config;
    my $currSection = "Default";

    # Parse each line
    foreach my $line (<CONFIG>) {
	# Ignore comments
	if($line =~ /;.*/) {
	    next;
	}
	# Update the section as we iterate along lines
	if($line =~ /\[(.*)\]/) {
	    $currSection = $1;
	    print "Found $currSection\n";
	}
	# Store key value pairs in the appropriate section
	if($line =~ /(.*?)=(.*)/) {
	    $config{$currSection}{$1} = $2;
	}
    }

    return %config;
}

# Parse the user/section configs
my %userConfig = parseConfig("assetUser.ini");
my %config = parseConfig("assetVersions.ini");

# Store the base path
my $basePath = $userConfig{"General"}{"RootPath"};
if(!$basePath) { $basePath = "DON'T COPY"; }

# Find the maximum/current session
my $maxSection;
foreach my $section (keys %config) {
    if(exists $config{$section}{"Current"} && $config{$section}{"Current"} =~ /(true|TRUE|True)/) {
	$maxSection = $section;
	last;
    }
    elsif($section =~ /(General|Default)/) {
	next;
    }
    elsif(!$maxSection || $section ge $maxSection) {
	$maxSection = $section;
    }
}

# Construct the magical copy command
my $command = "cp -rvu ".$basePath.$config{$maxSection}{"Path"}."/* Assets/";

# Tell the user what we're doing
print "Choosing $maxSection\n";
print("Executing '".$command."'");

# Do all the things
exec $command;
