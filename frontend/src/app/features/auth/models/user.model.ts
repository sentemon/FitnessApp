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
  lastSeenAt: Date;
  followersCount: number;
  followingCount: number;
  imageUrl: string;
  createdAt: Date;
}
