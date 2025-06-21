export interface LoginRequest {
  companyDB: string;
  userName: string;
  password: string;
}

export interface LoginResponse {
  sessionId: string;
  routeId: string;
}