# 🚀 CI/CD Setup Guide for Render

Complete guide to set up **automatic deployment pipeline** with GitHub Actions and Render.

## 📋 What You'll Get

- ✅ **Automatic builds** on every push
- ✅ **Docker image validation**
- ✅ **Automated deployment** to Render
- ✅ **Health checks** after deployment
- ✅ **Professional CI/CD pipeline**

---

## ⚡ Quick Setup (5 Steps)

### Step 1: Add Files to Your Repository

Add these 3 files to your project:

**1. Dockerfile** (root directory)
```dockerfile
# Replace your existing Dockerfile with the optimized version
# See Dockerfile-Final.txt
```

**2. `.github/workflows/deploy.yml`** (create this path)
```yaml
# Copy content from ci-cd-render.yml
```

**3. `render.yaml`** (root directory - already have it!)
```yaml
# Keep your existing render.yaml
```

### Step 2: Deploy to Render (First Time)

1. Go to [https://render.com](https://render.com)
2. Sign in with GitHub
3. **New +** → **Web Service**
4. Connect your `storefront-api` repository
5. Configure:
   ```
   Name: storefront-api
   Runtime: Docker
   Branch: main
   Dockerfile Path: ./Dockerfile
   Instance Type: Free
   ```
6. Click **"Create Web Service"**

Wait 3-5 minutes for first deployment.

### Step 3: Get Deploy Hook from Render

1. In Render Dashboard → Your Service (`storefront-api`)
2. **Settings** tab
3. Scroll to **"Deploy Hook"**
4. Click **"Create Deploy Hook"**
5. Copy the URL (looks like: `https://api.render.com/deploy/srv-xxxxx?key=yyyyy`)

### Step 4: Add GitHub Secret

1. Go to your GitHub repository
2. **Settings** → **Secrets and variables** → **Actions**
3. Click **"New repository secret"**
4. Add:
   - **Name:** `RENDER_DEPLOY_HOOK_URL`
   - **Value:** Paste the deploy hook URL from Step 3
5. Click **"Add secret"**

### Step 5: Push Your Code

```bash
# Add the new files
git add .github/workflows/deploy.yml
git add Dockerfile
git add render.yaml

# Commit
git commit -m "Add CI/CD pipeline for Render deployment"

# Push to trigger deployment!
git push origin main
```

---

## 🎉 Done!

Go to **Actions** tab in GitHub to watch your pipeline run!

You'll see:
1. ✅ **Build & Test** - Compiles your code
2. ✅ **Docker Build** - Creates container image
3. ✅ **Deploy** - Triggers Render deployment
4. ✅ **Validate** - Checks API is responding

---

## 📊 What Happens on Every Push

```
Push to GitHub (main branch)
    ↓
GitHub Actions starts
    ↓
Job 1: Build & Test
  ├─ Setup .NET 8
  ├─ Restore packages
  ├─ Build solution
  └─ Run tests
    ↓
Job 2: Docker Build
  ├─ Build Docker image
  ├─ Validate image
  └─ Cache for faster builds
    ↓
Job 3: Deploy to Render
  ├─ Trigger deploy hook
  └─ Render rebuilds & deploys
    ↓
Job 4: Validate Deployment
  ├─ Wait 90 seconds
  ├─ Health check
  └─ Report success
    ↓
✅ API is live!
```

**Total time:** 5-7 minutes from push to live! 🚀

---

## 🔍 View Pipeline Status

### In GitHub:

1. Go to **Actions** tab
2. See all workflow runs
3. Click any run to see details
4. Green checkmarks = success! ✅

### In Render:

1. Dashboard → Your Service
2. **Events** tab shows deployments
3. **Logs** tab shows build logs

---

## 🧪 Test Your CI/CD

Make a simple change:

```bash
# Edit README
echo "# Testing CI/CD" >> README.md

# Commit and push
git add README.md
git commit -m "Test CI/CD pipeline"
git push

# Watch the magic happen!
# GitHub Actions → Build → Deploy → Live in 5 minutes!
```

---

## 📝 Update Your README

Add this impressive section:

```markdown
## 🚀 CI/CD Pipeline

This project includes a **complete CI/CD pipeline** using GitHub Actions and Render:

### Pipeline Stages

1. **Build & Test** - Validates code compilation
2. **Docker Build** - Creates optimized container image  
3. **Deploy** - Automatic deployment to Render
4. **Validation** - Health checks post-deployment

### Deployment

Every push to `main` automatically:
- ✅ Builds the application
- ✅ Runs tests
- ✅ Creates Docker container
- ✅ Deploys to production
- ✅ Validates deployment

**Live in 5 minutes from code push!**

### View Pipeline

[![CI/CD Pipeline](https://github.com/YOUR_USERNAME/storefront-api/actions/workflows/deploy.yml/badge.svg)](https://github.com/YOUR_USERNAME/storefront-api/actions)

🔗 **Live Demo:** https://storefront-api.onrender.com/swagger
```

---

## 🎯 What This Demonstrates

Including this CI/CD pipeline shows:

✅ **DevOps Knowledge**
- Continuous Integration
- Continuous Deployment
- Automated testing
- Docker containerization

✅ **Professional Practices**
- Multi-stage builds
- Security (non-root user)
- Health checks
- Automated validation

✅ **Cloud Deployment**
- Platform-as-a-Service (PaaS)
- Environment configuration
- Production readiness

**This seriously sets you apart!** 🔥

---

## 🆘 Troubleshooting

### Issue: Workflow fails at "Build & Test"

**Check:**
- All .csproj files are committed
- Solution file is in root
- Build works locally: `dotnet build`

### Issue: Workflow fails at "Docker Build"

**Check:**
- Dockerfile is in root directory
- All COPY paths are correct
- Build works locally: `docker build -t test .`

### Issue: Workflow fails at "Deploy"

**Check:**
- `RENDER_DEPLOY_HOOK_URL` secret is set correctly
- Deploy hook URL is valid (test with curl)
- Render service exists and is configured for Docker

### Issue: Deployment successful but API not responding

**Check:**
- Render logs: Dashboard → Logs
- PORT environment variable binding
- Application starts successfully in Render

---

## 🎓 Advanced: Add Build Badge

Show off your CI/CD with a badge in your README:

```markdown
![CI/CD](https://github.com/YOUR_USERNAME/storefront-api/actions/workflows/deploy.yml/badge.svg)
```

This displays the current status of your pipeline! 🏅

---

## 💰 Costs

**Everything is FREE:**

- ✅ GitHub Actions: 2,000 minutes/month free
- ✅ Render: 750 hours/month free
- ✅ Docker Hub: Unlimited public images

**Total cost: $0.00** 💰

---

## 🎉 Summary

You now have:

✅ **Professional CI/CD pipeline**
✅ **Automated deployments**
✅ **Docker containerization**
✅ **Health monitoring**
✅ **Zero cost**

**This will seriously impress the reviewers!** 🔥

Every code change is:
1. Automatically tested
2. Automatically built
3. Automatically deployed
4. Automatically validated

**That's production-grade DevOps!** 🏆

---

## 📞 Quick Reference

### Useful Commands

```bash
# View workflow runs
gh run list

# Watch a workflow run
gh run watch

# View workflow logs
gh run view <run-id> --log

# Trigger manual deployment
gh workflow run deploy.yml
```

---

**Ready to set it up? Follow the 5 steps above!** 🚀

**Questions? Check the troubleshooting section!** 💪
