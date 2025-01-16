import { Component } from '@angular/core';
import {FormGroup} from "@angular/forms";
import {ContentType} from "../../../../core/enums/content-type.enum";

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrl: './create-post.component.scss'
})
export class CreatePostComponent {
  postForm!: FormGroup;

  submitPost() {

  }

  protected readonly ContentType = ContentType;
}
