namespace csharplox;

abstract class Expr{
    public interface IVisitor<T> {
        public T VisitBinaryExpr(Binary expr);
        public T VisitGroupingExpr(Grouping expr);
        public T VisitLiteralExpr(Literal expr);
        public T VisitUnaryExpr(Unary expr);
    }
    public class Binary : Expr{
        private readonly Expr left;
        private readonly Token op;
        private readonly Expr right;

        Binary (Expr left, Token op, Expr right){
            this.left = left;
            this.op = op;
            this.right = right;
        }

        public override T Accept<T>(IVisitor<T> visitor){
            return visitor.VisitBinaryExpr(this);
        }
    }

    public class Grouping : Expr{
        private readonly Expr expression;

        Grouping (Expr expression){
            this.expression = expression;
        }

        public override T Accept<T>(IVisitor<T> visitor){
            return visitor.VisitGroupingExpr(this);
        }
    }

    public class Literal : Expr{
        private readonly Object value;

        Literal (Object value){
            this.value = value;
        }

        public override T Accept<T>(IVisitor<T> visitor){
            return visitor.VisitLiteralExpr(this);
        }
    }

    public class Unary : Expr{
        private readonly Token op;
        private readonly Expr right;

        Unary (Token op, Expr right){
            this.op = op;
            this.right = right;
        }

        public override T Accept<T>(IVisitor<T> visitor){
            return visitor.VisitUnaryExpr(this);
        }
    }


    public abstract T Accept<T>(IVisitor<T> visitor);
}
