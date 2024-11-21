import {ContentType} from "../../../core/enums/content-type.enum";

export interface CreatePostDto {
  title: string;
  description: string;
  contentUrl: string;
  contentType: ContentType
}
