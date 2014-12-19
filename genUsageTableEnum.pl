#!/usr/local/bin/perl
#Developed by Stéphane Lenclud
#Generate C# enumeration from parsing Hid Usage Table
#See ../data/gl.h
#Usage example
#perl -S genUsageTableEnum.pl ../data/gl.h

use strict;
use warnings;	


my $inputFile = $ARGV[0];

#my $dummy="lala";
#$dummy=~s/(^\w)/uc($1)/e;
#print "$dummy";
#exit(0);

#Open input file
open INPUT, "< $inputFile" or die "Can't read $inputFile\n";
my @lines = <INPUT>;
close INPUT;


my %hash = ();

my $count=0;
foreach my $line(@lines)
	{
	#if ($line=~ /^\#\s*define\s+(.+?)\s+([a-fA-FxX\d]+?)\s*$/)	
	if ($line=~ /^([a-fA-FxX\d]+)(.+)\s+\w+\s+15\..*$/)	
		{
		my $string=$2;
		my $value=$1;
		
		my $varName=FormatVarName($string);	

		
		$hash{$string}=$value;
		
		print "$varName = 0x$value,\n";			
		}
	else
		{
		#print "NO MATCH $line\n";
		}
	}

exit(0);	
	
#Output in sorted order	
for my $string ( sort keys %hash )
	{	
    #print "_S8(\"$string\"),$hash{$string}, //$count\n";
    print "_S8(\"$string\"),$string, //$count\n";						
    $count++;	
    }	
	
	
print "$count const found.\n";	
	
exit(0);

#	

sub FormatVarName
	{
	my $text=$_[0];	
	my $varName="";
	Trim($text);
	#Make sure AC ends up as AppCtrl
	$text=~s/(^AC)/App Ctrl/;
	#Make sure AL ends up as AppLaunch
	$text=~s/(^AL)/App Launch/;		
	#Replace / by white-space
	$text=~s/\// /g;
	#Replace + with Plus
	$text=~s/\+/Plus/g;
	#Replace - with white-space
	$text=~s/-/ /g;

	
	$text=lc($text);
	while ($text=~/(\w+)\s+(.+)/)
		{		
		my $word=$1;
		$text=$2;
		#upper case the first letter
		$word=~s/(^\w)/uc($1)/e;	
		$varName.=$word;		
		}
	
	$text=~s/(^\w)/uc($1)/e;					
	$varName.=$text;		
	#get ride of -			
	$varName=~s/-(\w)/uc($1)/e;

	return $varName;
	}
	
sub Trim
	{
	$_[0] =~ s/^\s+//; #Trim leading space and line return char
	$_[0] =~ s/\s+$//; #Trim trailling space and line return char
	}	

