pipeline {
    agent any

    environment {
        DOCKER_CREDENTIALS_ID = 'DOCKER_HUB_ID'
        DOCKER_API_IMAGE = 'only1adwy/todo-api'
        DOCKER_CLIENT_IMAGE = 'only1adwy/todo-client'
        BRANCH = 'main'
        REPO_URL = 'https://github.com/moheladwy/TodoFullstack.git'
        BUILD_TAG = "V1.8.${env.BUILD_NUMBER}"
        
        // Client environment variables
        VITE_SERVER_URL = credentials('VITE_SERVER_URL')
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: "${BRANCH}", url: "${REPO_URL}"
            }
        }
        
        stage('Build API Docker Image') {
            steps {
                script {
                    // Build API Docker image
                    sh """
                        docker build -t ${DOCKER_API_IMAGE}:${BUILD_TAG} server/
                        docker tag ${DOCKER_API_IMAGE}:${BUILD_TAG} ${DOCKER_API_IMAGE}:latest
                    """
                }
            }
        }

        stage('Build Client Docker Image') {
            steps {
                script {
                    // Build Client Docker image with build arguments
                    sh """
                        docker build \
                        --build-arg VITE_SERVER_URL=${VITE_SERVER_URL} \
                        -t ${DOCKER_CLIENT_IMAGE}:${BUILD_TAG} client/
                        
                        docker tag ${DOCKER_CLIENT_IMAGE}:${BUILD_TAG} ${DOCKER_CLIENT_IMAGE}:latest
                    """
                }
            }
        }
        
        stage('Login to Docker Hub') {
            steps {
                script {
                    withCredentials([usernamePassword(credentialsId: DOCKER_CREDENTIALS_ID, passwordVariable: 'DOCKER_PASSWORD', usernameVariable: 'DOCKER_USERNAME')]) {
                        sh 'echo "$DOCKER_PASSWORD" | docker login -u "$DOCKER_USERNAME" --password-stdin'
                    }
                }
            }
        }
        
        stage('Push Docker Images to Docker Hub') {
            steps {
                script {
                    // Push API images
                    sh """
                        docker push ${DOCKER_API_IMAGE}:${BUILD_TAG}
                        docker push ${DOCKER_API_IMAGE}:latest
                    """
                    
                    // Push Client images
                    sh """
                        docker push ${DOCKER_CLIENT_IMAGE}:${BUILD_TAG}
                        docker push ${DOCKER_CLIENT_IMAGE}:latest
                    """
                }
            }
        }
    }

    post {
        success {
            echo 'Build and Docker Push completed successfully!'
        }
        failure {
            echo 'Build or Docker Push failed.'
        }
        always {
            // Clean up Docker images to free up space
            sh """
                docker rmi ${DOCKER_API_IMAGE}:${BUILD_TAG} ${DOCKER_API_IMAGE}:latest || true
                docker rmi ${DOCKER_CLIENT_IMAGE}:${BUILD_TAG} ${DOCKER_CLIENT_IMAGE}:latest || true
            """
        }
    }
}
