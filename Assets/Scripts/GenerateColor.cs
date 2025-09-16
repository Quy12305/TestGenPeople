using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GenerateColor : MonoBehaviour
{
    public Car       victimCar;
    public Car       blockerCar;
    public List<Car> allCars = new();

    public List<Color> GenerateSequence()
    {
        var grouped = this.allCars
            .GroupBy(car => car.numBlockers)
            .OrderBy(g => g.Key)
            .ToList();

        List<Color> finalSequence = new();

        List<Color> delayedPassengers = new();

        foreach (var group in grouped)
        {
            List<Color> groupSequence = new();

            foreach (var car in group)
            {
                if (car == this.blockerCar)
                {
                    int keep  = car.capacity / 2;
                    int delay = car.capacity - keep;

                    for (int i = 0; i < keep; i++) groupSequence.Add(car.color);

                    for (int i = 0; i < delay; i++) delayedPassengers.Add(car.color);
                }
                else
                {
                    for (int i = 0; i < car.capacity; i++) groupSequence.Add(car.color);
                }
            }

            groupSequence = this.Shuffle(groupSequence);

            if (group.Any(c => c == this.victimCar))
            {
                groupSequence.AddRange(delayedPassengers);
            }

            finalSequence.AddRange(groupSequence);
        }

        return finalSequence;
    }

    private List<Color> Shuffle(List<Color> list)
    {
        System.Random rand = new();
        int           n    = list.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
        return list;
    }

    private void Start()
    {
        this.allCars = new List<Car>
        {
            new Car { color = Color.blue, capacity    = 4, numBlockers  = 0 },
            new Car { color = Color.yellow, capacity  = 6, numBlockers  = 1 },
            new Car { color = Color.red, capacity     = 8, numBlockers  = 2 },
            new Car { color = Color.magenta, capacity = 10, numBlockers = 3 },
        };

        var sequence = this.GenerateSequence();

        Debug.Log(sequence.Count);
        Debug.Log(string.Join(", ", sequence.Select(c => c.ToString())));
    }
}