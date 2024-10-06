interface PieceMessage
{
  x: number;
  y: number;
  type: string;
  playerId: number;
}

interface EvolutionMessage
{
  x: number;
  y: number;
  playerId: number;
}

interface TilePositionMessage
{
  arrayX: number;
  arrayY: number;
  transformX: number;
  transformY: number;
}