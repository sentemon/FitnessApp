import { Component } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidationErrors,
  Validators
} from "@angular/forms";
import { Router } from "@angular/router";
import { AuthService } from "../../services/auth.service";
import {StorageService} from "../../../../core/services/storage.service";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: '../auth.scss'
})
export class RegisterComponent {
  registerForm: FormGroup;
  errorMessage: string = "";

  constructor(private authService: AuthService, private storageService: StorageService, private formBuilder: FormBuilder, private router: Router) {
    this.registerForm = this.formBuilder.group({
      firstName: ['', [
        Validators.required,
        Validators.maxLength(128),
        Validators.pattern(/^[A-Za-z]+$/)
      ]],
      lastName: ['', [
        Validators.required,
        Validators.maxLength(128),
        Validators.pattern(/^[A-Za-z]+$/)
      ]],
      username: ['', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(30),
        Validators.pattern(/^[A-Za-z][A-Za-z0-9._]*$/)
      ]],
      email: ['', [
        Validators.required,
        Validators.email
      ]],
      password: ['', [
        Validators.required,
        Validators.minLength(6),
        Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).+$/)
      ]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });
  }

  onRegister(): void {
    if (this.registerForm.valid) {
      this.authService.register(
        this.registerForm.value.firstName,
        this.registerForm.value.lastName,
        this.registerForm.value.username,
        this.registerForm.value.email,
        this.registerForm.value.password
      ).subscribe(async result => {
        if (result.isSuccess) {
          await this.storageService.init();
          await this.router.navigate(["/"]);
        } else {
          this.errorMessage = result.error.message;
        }
      });
    } else {
      this.errorMessage = this.getFormValidationErrors();
    }
  }

  private getFormValidationErrors(): string {
    const messages: string[] = [];

    Object.keys(this.registerForm.controls).forEach(key => {
      const controlErrors: ValidationErrors | null | undefined = this.registerForm.get(key)?.errors;
      if (controlErrors != null) {
        Object.keys(controlErrors).forEach(errorKey => {
          switch (errorKey) {
            case 'required':
              messages.push(`${this.prettyName(key)} is required`);
              break;
            case 'minlength':
              messages.push(`${this.prettyName(key)} must be at least ${controlErrors['minlength'].requiredLength} characters`);
              break;
            case 'maxlength':
              messages.push(`${this.prettyName(key)} cannot exceed ${controlErrors['maxlength'].requiredLength} characters`);
              break;
            case 'email':
              messages.push(`Invalid email format`);
              break;
            case 'pattern':
              messages.push(this.getPatternMessage(key));
              break;
            default:
              messages.push(`${this.prettyName(key)} is invalid`);
          }
        });
      }
    });

    if (this.registerForm.errors?.['passwordMismatch']) {
      messages.push('Passwords do not match');
    }

    return messages.join('. ');
  }

  private prettyName(key: string): string {
    switch (key) {
      case 'firstName': return 'First name';
      case 'lastName': return 'Last name';
      case 'username': return 'Username';
      case 'email': return 'Email';
      case 'password': return 'Password';
      case 'confirmPassword': return 'Confirm Password';
      default: return key;
    }
  }

  private getPatternMessage(key: string): string {
    switch (key) {
      case 'firstName':
      case 'lastName':
        return `${this.prettyName(key)} can only contain letters`;
      case 'username':
        return `Username must start with a letter and can contain letters, numbers, dots, or underscores`;
      case 'password':
        return `Password must contain at least one uppercase letter, one lowercase letter, and one number`;
      default:
        return `${this.prettyName(key)} is invalid`;
    }
  }

  private passwordMatchValidator(group: AbstractControl): ValidationErrors | null {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { passwordMismatch: true };
  }
}
