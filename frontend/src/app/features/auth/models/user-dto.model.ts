export interface UserDto {
  id: string;
  firstName: string;
  lastName: string;
  username: string;
  lastSeenAt: Date
  imageUrl: string;
  followersCount: number;
  followingCount: number;
}
