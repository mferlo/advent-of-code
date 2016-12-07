#!perl
use strict;

part2();

sub part1() {
    while (<>) {
        my $brackets = 0;
        my $found = 0;
        foreach my $part (split /[\[\]]/) {
            if ($part =~ /(.)(?!\1)(.)\2\1/) {
                if ($brackets) {
                    $found = 0;
                    last;
                } else {
                    $found = 1;
                }
            }
            $brackets = 1 - $brackets;
        }
        print if $found;
    }
}

sub part2() {
    while (<>) {
        my %ABAs = {};
        my %BABs = {};
        my $inside = 0;
        foreach my $part (split /[\[\]]/) {
            while ($part =~ /(.)(?!\1)(?=(.)\1)/g) {
                if ($inside) {
                    $ABAs{"$1$2$1"} = "$2$1$2";
                } else {
                    $BABs{"$1$2$1"} = 1;
                }
            }
            $inside = 1 - $inside;
        }

        my $found = 0;
        foreach my $potentialBAB (values %ABAs) {
            $found ||= $BABs{$potentialBAB};
        }
        print if $found;
    }
}
