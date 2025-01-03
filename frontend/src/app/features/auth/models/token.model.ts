export interface Token {
  accessToken: string
  expiresIn: number
  refreshExpiresIn: number
  refreshToken: string
  tokenType: string
  notBeforePolicy: number
  sessionState: string
  scope: string
}
