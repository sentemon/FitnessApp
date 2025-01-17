import { Component } from '@angular/core';
import {FormBuilder, FormGroup} from "@angular/forms";
import {ContentType} from "../../../../core/enums/content-type.enum";
import {PostService} from "../../services/post.service";

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrl: './create-post.component.scss'
})
export class CreatePostComponent {
  postForm!: FormGroup;
  protected contentTypes = ContentType;

  constructor(private fb: FormBuilder, private postService: PostService) {
    this.postForm = fb.group({
      title: "",
      description: "",
      contentType: ContentType.Text,
      file: null
    })
  }

  submitPost() {
    this.postService.createPost(this.title, this.description, this.contentType, this.file);
  }

  protected get title(): string {
    return this.postForm.get("title")?.value;
  }

  protected get description(): string {
    return this.postForm.get("description")?.value;
  }

  protected get contentType(): ContentType {
    return this.postForm.get("contentType")?.value;
  }

  protected get file(): File {
    return this.postForm.get("file")?.value;
  }
}
