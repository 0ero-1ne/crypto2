using Lab11.Elliptical;

var curve = new EllipticalCurve(-1, 1, 751);

var timer = System.Diagnostics.Stopwatch.StartNew();
var points = curve.GetEllipticalPoints(36, 70);
timer.Stop();

foreach (var item in points)
{
    Console.WriteLine(item);
}

Console.WriteLine(timer);
