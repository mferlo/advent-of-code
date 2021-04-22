// Part 2 refactored result -- solution was obtained from 3rd party.

bool isPrime(b) {
    for (int d = 2; d != b; d++) {
        for (int e = 2; e != b; e++) {
            if (d*e == b) {
                return false;
            }
        }
    }
    return true;
}

int algo() {
    for (int b = 108400; b <= 125400; b += 17) {
        if (isPrime(b)) {
            h++;
        }
    }
    return h;
}
