# Cloudflared Configuration

This directory contains configuration files for [Cloudflared](https://github.com/cloudflare/cloudflared), the command-line tool for Cloudflare Tunnel.

## Files

- `config.yml`: Main configuration file for Cloudflared.

## Usage

1. **Install Cloudflared**  
    Follow instructions at [Cloudflared Installation Guide](https://developers.cloudflare.com/cloudflare-one/connections/connect-apps/install-and-setup/installation/).

2. **Configure Tunnel**  
    Edit `config.yml` to set up your tunnel, ingress rules, and credentials.

3. **Run Cloudflared**  
    ```bash
    cloudflared tunnel run <TUNNEL-NAME>
    ```

## Reference

- [Cloudflared Documentation](https://developers.cloudflare.com/cloudflare-one/connections/connect-apps/)
- [Configuration Reference](https://developers.cloudflare.com/cloudflare-one/connections/connect-apps/configuration/)
