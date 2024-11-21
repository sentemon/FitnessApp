import {ContentType} from "../../../core/enums/content-type.enum";

export interface Post {
  id: string;
  title: string;
  description: string;
  contentUrl: string;
  contentType: ContentType;
  likeCount: number;
  commentCount: number;
  createdAt: Date;
}
