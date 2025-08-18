import { Component } from '@angular/core';
import {UserService} from "../../services/user.service";
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators} from "@angular/forms";

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.scss'
})
export class ResetPasswordComponent {
  resetPasswordForm: FormGroup;
  errorMessage: string = "";

  constructor(private userService: UserService, private formBuilder: FormBuilder) {
    this.resetPasswordForm = formBuilder.group({
      oldPassword: ['', Validators.required, Validators.minLength(6)],
      newPassword: ['', Validators.required, Validators.minLength(6)],
      confirmNewPassword: ['', Validators.required, Validators.minLength(6)]
    }, { validators: this.passwordMatchValidator });
  }

  onResetPassword() {
    this.userService.resetPassword(
      this.resetPasswordForm.get("oldPassword")?.value,
      this.resetPasswordForm.get("newPassword")?.value,
      this.resetPasswordForm.get("confirmNewPassword")?.value
    ).subscribe(result => {
      if (!result.isSuccess) {
        this.errorMessage = result.error.message;
      }
    });
  }

  private passwordMatchValidator(group: AbstractControl): ValidationErrors | null {
    const newPassword = group.get('newPassword')?.value;
    const confirmNewPassword = group.get('confirmNewPassword')?.value;
    return newPassword === confirmNewPassword ? null : { passwordMismatch: true };
  }

}
