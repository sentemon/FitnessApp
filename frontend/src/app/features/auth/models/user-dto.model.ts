export interface UserDto {
  id: string;
  firstName: string;
  lastName: string;
  username: string;
  bio: string;
  lastSeenAt: Date
  imageUrl: string;
  followersCount: number;
  followingCount: number;
}
