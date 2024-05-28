using System.Windows;

namespace SecantMethod;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnCalculateClick(object sender, RoutedEventArgs e)
    {
        string function = TxtFunction.Text;
        var x0 = double.Parse(TxtX0.Text);  // Ensure the name matches the XAML definition
        var x1 = double.Parse(TxtX1.Text);  // Ensure the name matches the XAML definition
        var maxIterations = int.Parse(TxtIterations.Text);


        var results = new List<IterationResult>();
        for (var i = 0; i < maxIterations; i++)
        {
            var fx0 = EvaluateFunction(function, x0);
            var fx1 = EvaluateFunction(function, x1);
            var x2 = x1 - fx1 * (x1 - x0) / (fx1 - fx0);

            results.Add(new IterationResult
            {
                Iteration = i + 1,
                X0 = x0,
                X1 = x1,
                XRoot = x2,
                Error = Math.Abs(x2 - x1)
            });

            x0 = x1;
            x1 = x2;
        }

        ResultsGrid.ItemsSource = results;
    }

    private double EvaluateFunction(string function, double x)
    {
        /*// Evaluate the function at x using a library like Math.NET Symbolics or similar
        // Placeholder for actual implementation
        // return 0; This should be replaced with actual function evaluation
        var expr = Expr.Parse(function);  // Parse the function
        var symbol = Expr.Variable("x");  // Define the variable
        var subs = expr.Substitute(symbol, x);  // Substitute the variable with the actual value
        return Evaluate.Real(subs);  // Evaluate the expression*/
        // Parse the function to a symbolic expression
        var expr = Expr.Parse(function);

        // Create a dictionary for the variable substitution
        var variables = new Dictionary<string, FloatingPoint>()
        {
            { "x", x }
        };

        // Evaluate the expression with the substituted values
        var result = expr.Evaluate(variables);

        // Convert the result to a double
        return (double)result.RealValue;
    }
}
