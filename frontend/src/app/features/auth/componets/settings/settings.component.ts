import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {UserService} from "../../services/user.service";

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
    private userService: UserService
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

    const formData = new FormData();
    formData.append('firstName', this.profileForm.value.firstName);
    formData.append('lastName', this.profileForm.value.lastName);
    formData.append('username', this.profileForm.value.username);
    formData.append('email', this.profileForm.value.email);
    if (this.profileForm.value.password) {
      formData.append('password', this.profileForm.value.password);
    }
    if (this.selectedAvatar) {
      formData.append('avatar', this.selectedAvatar);
    }

    this.userService.update(formData).subscribe({
      next: () => {
        this.loading = false;
        alert('Profile updated successfully!');
      },
      error: () => {
        this.loading = false;
        alert('Failed to update profile!');
      }
    });
  }

  deleteAccount() {
    if (confirm('Are you sure you want to delete?')) {
      this.userService.deleteUser().subscribe(() => {
        alert('Account deleted successfully!');
        // redirect
      });
    }
  }
}
