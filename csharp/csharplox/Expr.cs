namespace csharplox;

abstract class Expr{
    class Binary : Expr{
        private readonly Expr left;
        private readonly Token op;
        private readonly Expr right;

        Binary (Expr left, Token op, Expr right){
            this.left = left;
            this.op = op;
            this.right = right;
        }

    }
    class Grouping : Expr{
        private readonly Expr expression;

        Grouping (Expr expression){
            this.expression = expression;
        }

    }
    class Literal : Expr{
        private readonly Object value;

        Literal (Object value){
            this.value = value;
        }

    }
    class Unary : Expr{
        private readonly Token op;
        private readonly Expr right;

        Unary (Token op, Expr right){
            this.op = op;
            this.right = right;
        }

    }
}
