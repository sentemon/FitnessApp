import { Component } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {AuthService} from "../../services/auth.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: '../auth.scss'
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage: string = "";

  constructor(private authService: AuthService, private formBuilder: FormBuilder, private router: Router) {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });
  }

  onLogin(): void {
    if (this.loginForm.valid) {
      this.authService.login(this.loginForm.value.email, this.loginForm.value.password).subscribe(result => {
        if (result.isSuccess) {
          this.router.navigate(["/"]);
        } else {
          this.errorMessage = result.error.message;
        }
      });
    } else {
      console.log('Form is invalid', this.loginForm.errors);
    }
  }
}
