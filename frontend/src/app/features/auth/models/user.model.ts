export interface User {
  id: string;
  firstName: string;
  lastName: string;
  username: {
    value: string;
  };
  email: {
    value: string;
  };
  followersCount: number;
  followingCount: number;
  imageUrl: string;
}
