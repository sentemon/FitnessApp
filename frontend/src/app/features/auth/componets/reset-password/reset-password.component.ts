import { Component } from '@angular/core';
import {UserService} from "../../services/user.service";
import {AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators} from "@angular/forms";
import {Result} from "../../../../core/types/result/result.type";

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.scss'
})
export class ResetPasswordComponent {
  resetPasswordForm: FormGroup;
  messageResult: Result<string> | null = null;
  message: string = "";

  constructor(private userService: UserService, formBuilder: FormBuilder) {
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
      this.messageResult = result;

      if (!result.isSuccess) {
        this.message = result.error.message;
        return;
      }

      this.message = result.response;
      this.resetPasswordForm.reset();
    });
  }

  private passwordMatchValidator(group: AbstractControl): ValidationErrors | null {
    const newPassword = group.get('newPassword')?.value;
    const confirmNewPassword = group.get('confirmNewPassword')?.value;
    return newPassword === confirmNewPassword ? null : { passwordMismatch: true };
  }
}
