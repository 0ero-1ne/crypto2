#include <stdio.h>
#include <stdlib.h>
#include <memory.h>

int *Sieve(int n)
{
    int *array = (int *)malloc(n * sizeof(int));

    if (array == NULL)
    {
        printf("Memory allocating error");
        return NULL;
    }

    memset(array, 0, n * sizeof(int));

    array[0] = 1;
    array[1] = 1;

    for (int i = 2; i * i < n; i++)
    {
        if (array[i] == 0)
        {
            for (int j = i * i; j <= n; j += i)
            {
                array[j] = 1;
            }
        }
    }

    return array;
}

int main()
{
    int m = 521;
    int n = 553;
    int counter = 0;

    int *array = Sieve(n + 1);

    printf("Interval - [2, n]\n");

    for (int i = 0; i < n + 1; i++)
    {
        if (array[i] == 0)
        {
            printf("%d ", i);
            counter++;
        }
    }

    printf("\nNumber of primal numbers: %d\n\n", counter);
    counter = 0;

    printf("Interval - [m, n]\n");

    for (int i = m; i < n + 1; i++)
    {
        if (array[i] == 0)
        {
            printf("%d ", i);
            counter++;
        }
    }

    printf("\nNumber of primal numbers: %d\n", counter);

    free(array);

    return 0;
}