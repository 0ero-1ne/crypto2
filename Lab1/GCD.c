#include <stdio.h>
#include <stdlib.h>

int GCD(int first, int second)
{
    if (first <= 0 || second <= 0)
    {
        return -1;
    }

    int remainder = 0;

    while (first % second != 0)
    {
        remainder = first % second;
        first = second;
        second = remainder;
    }

    return remainder;
}

int main(int argc, char *argv[])
{
    if (argc < 3)
    {
        printf("No GCD arguments: 2 int requiered");
        return -1;
    }

    if (argc == 3)
    {
        printf("%d", GCD(atoi(argv[1]), atoi(argv[2])));
    }
    else
    {
        printf("%d", GCD(GCD(atoi(argv[1]), atoi(argv[2])), atoi(argv[3])));
    }

    return 0;
}