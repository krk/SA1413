using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace RoslynAnalyzers.Test
{
    [TestClass]
    public sealed class SA1413Verifier : CodeFixVerifier
    {
        private const string AnonymousObjectInitializer = "Anonymous Object Initializer";
        private const string EnumDeclaration = "Enum Declaration";
        private const string ObjectInitializer = "Object Initializer";

        private static DiagnosticResult GetResult(int line, int column) => new DiagnosticResult
        {
            Id = SA1413UseTrailingCommasInMultiLineInitializers.DiagnosticId,
            Locations = new[]
            {
                new DiagnosticResultLocation("Test0.cs", line, column)
            },
            Message = Resources.SA1413MessageFormat,
            Severity = DiagnosticSeverity.Warning
        };

        [TestMethod]
        public void Empty_Input_IsValid()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [TestCategory(AnonymousObjectInitializer)]
        public void MultiLine_AnonymousObjectInitializer_MultiMember_Comma()
        {
            var test = @"class A
{
    A()
    {
        var a = new
        {
            x = 1,
            y = 2,
        };
    }
}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [TestCategory(AnonymousObjectInitializer)]
        public void MultiLine_AnonymousObjectInitializer_MultiMember_NoComma()
        {
            var test = @"class A
{
    A()
    {
        var a = new
        {
            x = 1,
            y = 2
        };
    }
}";

            VerifyCSharpDiagnostic(test, GetResult(8, 13));

            var @fixed = @"class A
{
    A()
    {
        var a = new
        {
            x = 1,
            y = 2,
        };
    }
}";

            VerifyCSharpFix(test, @fixed);
        }

        [TestMethod]
        [TestCategory(AnonymousObjectInitializer)]
        public void MultiLine_AnonymousObjectInitializer_SingleMember_Comma()
        {
            var test = @"class A
{
    A()
    {
        var a = new
        {
            x = 1,
        };
    }
}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [TestCategory(AnonymousObjectInitializer)]
        public void MultiLine_AnonymousObjectInitializer_SingleMember_NoComma()
        {
            var test = @"class A
{
    A()
    {
        var a = new
        {
            x = 1
        };
    }
}";

            VerifyCSharpDiagnostic(test, GetResult(7, 13));

            var @fixed = @"class A
{
    A()
    {
        var a = new
        {
            x = 1,
        };
    }
}";

            VerifyCSharpFix(test, @fixed);
        }

        [TestMethod]
        [TestCategory(EnumDeclaration)]
        public void MultiLine_EnumDeclaration_MultiMember_Comma()
        {
            var test = @"enum A
{
    X, 
    Y, 
    Z,
}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [TestCategory(EnumDeclaration)]
        public void MultiLine_EnumDeclaration_MultiMember_NoComma()
        {
            var test = @"enum A
{
    X,
    Y,
    Z
}";

            VerifyCSharpDiagnostic(test, GetResult(5, 5));

            var @fixed = @"enum A
{
    X,
    Y,
    Z,
}";

            VerifyCSharpFix(test, @fixed);
        }

        [TestMethod]
        [TestCategory(EnumDeclaration)]
        public void MultiLine_EnumDeclaration_SingleMember_Comma()
        {
            var test = @"enum A
{
    X,
}";
            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [TestCategory(EnumDeclaration)]
        public void MultiLine_EnumDeclaration_SingleMember_NoComma()
        {
            var test = @"enum A
{
    X
}";

            VerifyCSharpDiagnostic(test, GetResult(3, 5));

            var @fixed = @"enum A
{
    X,
}";

            VerifyCSharpFix(test, @fixed);
        }

        [TestMethod]
        [TestCategory(ObjectInitializer)]
        public void MultiLine_ObjectInitializer_MultiMember_Comma()
        {
            var test = @"class A
{
    private int X;
    private int Y;

    A()
    {
        var a = new A() 
        {
            X = 1,
            Y = 2,
        };
    }
}";
            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [TestCategory(ObjectInitializer)]
        public void MultiLine_ObjectInitializer_MultiMember_NoComma()
        {
            var test = @"class A
{
    private int X;

    A()
    {
        var a = new A() 
        {
            X = 1,
            Y = 2
        };
    }
}";

            VerifyCSharpDiagnostic(test, GetResult(10, 13));

            var @fixed = @"class A
{
    private int X;

    A()
    {
        var a = new A() 
        {
            X = 1,
            Y = 2,
        };
    }
}";

            VerifyCSharpFix(test, @fixed);
        }

        [TestMethod]
        [TestCategory(ObjectInitializer)]
        public void MultiLine_ObjectInitializer_SingleMember_Comma()
        {
            var test = @"class A
{
    private int X;

    A()
    {
        var a = new A() 
        {
            X = 1,
        };
    }
}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [TestCategory(ObjectInitializer)]
        public void MultiLine_ObjectInitializer_SingleMember_NoComma()
        {
            var test = @"class A
{
    private int X;

    A()
    {
        var a = new A() 
        {
            X = 1
        };
    }
}";

            VerifyCSharpDiagnostic(test, GetResult(9, 13));

            var @fixed = @"class A
{
    private int X;

    A()
    {
        var a = new A() 
        {
            X = 1,
        };
    }
}";

            VerifyCSharpFix(test, @fixed);
        }

        [TestMethod]
        [TestCategory(AnonymousObjectInitializer)]
        public void SingleLine_AnonymousObjectInitializer_MultiMember_Comma()
        {
            var test = @"class A
{
    A()
    {
        var a = new { x = 1, y = 2, };
    }
}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [TestCategory(AnonymousObjectInitializer)]
        public void SingleLine_AnonymousObjectInitializer_MultiMember_NoComma()
        {
            var test = @"class A
{
    A()
    {
        var a = new { x = 1, y = 1 };
    }
}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [TestCategory(AnonymousObjectInitializer)]
        public void SingleLine_AnonymousObjectInitializer_SingleMember_Comma()
        {
            var test = @"class A
{
    A()
    {
        var a = new { x = 1, };
    }
}";

            VerifyCSharpDiagnostic(test);
        }


        [TestMethod]
        [TestCategory(AnonymousObjectInitializer)]
        public void SingleLine_AnonymousObjectInitializer_SingleMember_NoComma()
        {
            var test = @"class A
{
    A()
    {
        var a = new { x = 1 };
    }
}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [TestCategory(EnumDeclaration)]
        public void SingleLine_EnumDeclaration_MultiMember_Comma()
        {
            var test = @"enum A { X, Y, }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [TestCategory(EnumDeclaration)]
        public void SingleLine_EnumDeclaration_MultiMember_NoComma()
        {
            var test = @"enum A { X, Y }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [TestCategory(EnumDeclaration)]
        public void SingleLine_EnumDeclaration_SingleMember_Comma()
        {
            var test = @"enum A { X, }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [TestCategory(EnumDeclaration)]
        public void SingleLine_EnumDeclaration_SingleMember_NoComma()
        {
            var test = @"enum A { X }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [TestCategory(ObjectInitializer)]
        public void SingleLine_ObjectInitializer_MultiMember_Comma()
        {
            var test = @"class A
{
    private int X;

    A()
    {
        var a = new A() { X = 1, Y = 2, };
    }
}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [TestCategory(ObjectInitializer)]
        public void SingleLine_ObjectInitializer_MultiMember_NoComma()
        {
            var test = @"class A
{
    private int X;

    A()
    {
        var a = new A() { X = 1, Y = 2 };
    }
}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [TestCategory(ObjectInitializer)]
        public void SingleLine_ObjectInitializer_SingleMember_Comma()
        {
            var test = @"class A
{
    private int X;

    A()
    {
        var a = new A() { X = 1, };
    }
}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        [TestCategory(ObjectInitializer)]
        public void SingleLine_ObjectInitializer_SingleMember_NoComma()
        {
            var test = @"class A
{
    private int X;

    A()
    {
        var a = new A() { X = 1 };
    }
}";

            VerifyCSharpDiagnostic(test);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider() => new SA1413CodeFixProvider();

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() =>
            new SA1413UseTrailingCommasInMultiLineInitializers();
    }
}