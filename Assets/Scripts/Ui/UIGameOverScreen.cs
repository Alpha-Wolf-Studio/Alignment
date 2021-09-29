public class UIGameOverScreen
{
    private UIGameOverScreen() { } //Previene instanciar esta clase
    public static string GetGameOverText(DamageOrigin origin) 
    {
        var gameOverText = "";
        switch (origin)
        {
            case DamageOrigin.PLAYER:
                gameOverText = "Spamming shoot kills you.\nPay attention to your overload meter to the side\nof your aim marker.";
                break;
            case DamageOrigin.RAPTOR:
                gameOverText = "Raptor are balanced dinos.\nKeep your distance and kill them one by one.";
                break;
            case DamageOrigin.TRICERATOPS:
                gameOverText = "Triceratops fling you into the air after each hit.\nTry to evade after they start charging.";
                break;
            case DamageOrigin.DILOPHOSAURUS:
                gameOverText = "Dilophosaurus shoot energy bolts.\nAttack in between their range attacks.";
                break;
            case DamageOrigin.COMPSOGNATHUS:
                gameOverText = "Compsognathus are weak by they move in groups.\nMove constantly while attacking.";
                break;
            case DamageOrigin.WATER:
                gameOverText = "Robots don't like water.\n Stay away from water.";
                break;
            default:
                break;
        }
        return gameOverText;
    }

}
