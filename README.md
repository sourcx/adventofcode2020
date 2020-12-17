# adventofcode

C# code for 2020 challenges on <https://adventofcode.com>.

From <https://docs.microsoft.com/en-gb/dotnet/core/get-started>

```sh
dotnet new console --output adventofcode2020
aaaaand go !
```

## Day 17 learns

Enumerable.SelectMany

Projects each element of a sequence to an IEnumerable<T> and flattens the resulting sequences into one sequence.

```cs
// Create [-1, 0, -1] in 3 dimensions.
var offsets = Enumerable.Range(-1, 3) // range for z: -1, 0, 1
    .SelectMany(z => Enumerable.Range(-1, 3) // range for y: -1, 0, 1
        .SelectMany(y => Enumerable.Range(-1, 3) // range for x: -1, 0, 1
            .Select(x => (z, y, x)))); // create the tuple
```

Switch statement assignment short form.

```cs
var a = 42;

var b = a switch
{
    42 => true,
    _ => false
};
```
