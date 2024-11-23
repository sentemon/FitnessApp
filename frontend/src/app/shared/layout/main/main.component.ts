import { Component } from '@angular/core';
import {PostListComponent} from "../../../features/posts/components/post-list/post-list.component";

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [
    PostListComponent
  ],
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent {

}
