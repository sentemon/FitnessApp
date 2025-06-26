import {Component} from '@angular/core';
import {FormBuilder, FormGroup} from "@angular/forms";
import {ContentType} from "../../../../core/enums/content-type.enum";
import {PostService} from "../../services/post.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrl: './create-post.component.scss'
})
export class CreatePostComponent {
  postForm!: FormGroup;
  protected contentTypes = ContentType;
  private file!: File;

  private url!: string;

  constructor(private fb: FormBuilder, private postService: PostService, private router: Router) {
    this.postForm = fb.group({
      title: "",
      description: "",
      contentType: ContentType.Image,
      file: null,
    });
  }

  submitPost() {
    this.postService.createPost(this.title, this.description, this.contentType, this.file).subscribe(response => {
      this.postService.addPost({ ...response, contentUrl: this.contentUrl });
      this.router.navigate(["/"]);
    });
  }

  protected onChange(event: any): void {
    const target = event.target;
    const file = target.files?.item(0);

    if (file) {
      this.file = file;

      this.url = URL.createObjectURL(file);
    }
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

  protected get contentUrl(): string {
    return this.url;
  }

  protected readonly Date = Date;
  protected readonly ContentType = ContentType;
}
