export interface User {
  id: string;
  firstName: string;
  lastName: string;
  username: {
    value: string;
  };
  email: {
    value: string;
  }
  imageUrl: string;
}
