export interface DecodedToken{
  unique_name: string;
  nameid: string;
  email: string;
  role: string;
  nbf: number;
  exp: number;
  iat: number;
}
