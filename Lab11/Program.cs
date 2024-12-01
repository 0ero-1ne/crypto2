using Lab11.Elliptical;

var timer = System.Diagnostics.Stopwatch.StartNew();
var curve = new EllipticalCurve(-1, 1, 751);
timer.Stop();

Console.WriteLine(timer);
timer.Restart();
var points = curve.GetPointsRange(36, 70);
timer.Stop();

foreach (var item in points)
{
    Console.WriteLine(item);
}

Console.WriteLine(timer);