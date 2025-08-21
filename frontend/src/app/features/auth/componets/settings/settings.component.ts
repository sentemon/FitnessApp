import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {UserService} from "../../services/user.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.scss'
})
export class SettingsComponent implements OnInit {
  profileForm!: FormGroup;
  selectedAvatar: File | null = null;
  avatarPreview: string | ArrayBuffer | null = null;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit() {
    this.profileForm = this.fb.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      username: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.minLength(6)]]
    });

    this.userService.getCurrentUser().subscribe(result => {
      if (!result.isSuccess) {
        return;
      }

      this.profileForm.patchValue({
        firstName: result.response.firstName,
        lastName: result.response.lastName,
        username: result.response.username.value,
        email: result.response.email.value
      });
      this.avatarPreview = result.response.imageUrl;
    });
  }

  onAvatarChange(event: Event) {
    const file = (event.target as HTMLInputElement)?.files?.[0];
    if (file) {
      this.selectedAvatar = file;
      const reader = new FileReader();
      reader.onload = e => this.avatarPreview = reader.result;
      reader.readAsDataURL(file);
    }
  }

  saveChanges() {
    if (this.profileForm.invalid) return;
    this.loading = true;

    this.userService.update(
      this.profileForm.value.firstName,
      this.profileForm.value.lastName,
      this.profileForm.value.username,
      this.profileForm.value.email,
      this.selectedAvatar
    ).subscribe(result => {
      if (result.isSuccess) {
        this.loading = false;
      }
    });
  }

  deleteAccount() {
    if (confirm('Are you sure you want to delete?')) {
      this.userService.delete().subscribe(result => {
        if (result.response) {
          this.router.navigate(['/']);
        }
      });
    }
  }
}
