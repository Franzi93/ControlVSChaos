namespace Duality
{
    public class GameCell
    {
        public ECellType type = ECellType.Normal;
        public MoveableFigure figure = default;

        public bool Contains(EEnemyType enemyType)
        {
            return (figure && figure.enemyType.Equals(enemyType));
        }
        public bool Contains(ECharacterType characterType)
        {
            return (figure && figure.type.Equals(characterType));
        }
    }
}